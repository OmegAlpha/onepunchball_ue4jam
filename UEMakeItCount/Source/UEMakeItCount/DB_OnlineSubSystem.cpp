// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_OnlineSubSystem.h"
#include "../Plugins/Online/OnlineSubsystem/Source/Public/OnlineSubsystem.h"
#include "OnlineSessionInterface.h"
#include "OnlineSessionSettings.h"
#include "Engine/World.h"
#include "UEMakeItCountPlayerController.h"
#include "Engine/Engine.h"
#include "UObjectBase.h"
#include "DB_GameInstance.h"


const static FName SESSION_NAME = TEXT("GameSession_");
const static FName SERVER_NAME_SETTINGS_KEY = TEXT("ServerName");

// Sets default values
UDB_OnlineSubSystem::UDB_OnlineSubSystem()
{

}

void UDB_OnlineSubSystem::Init()
{
	OnlineSub = IOnlineSubsystem::Get();
	SessionInterface = OnlineSub->GetSessionInterface();

	UE_LOG(LogTemp, Warning, TEXT("[DB] Init Session and Interface"));

	SessionInterface->OnCreateSessionCompleteDelegates.AddUObject(this, &UDB_OnlineSubSystem::OnCreateSessionCompleteDelegate_Handle);
	SessionInterface->OnFindSessionsCompleteDelegates.AddUObject(this, &UDB_OnlineSubSystem::OnSessionsRefreshed_Handle);
	SessionInterface->OnJoinSessionCompleteDelegates.AddUObject(this, &UDB_OnlineSubSystem::OnJoinSessionComplete_Handle);
	SessionInterface->OnDestroySessionCompleteDelegates.AddUObject(this, &UDB_OnlineSubSystem::OnDestroySessionComplete_Handle);
	
}

void UDB_OnlineSubSystem::AddPlayerToSession(APlayerController * LogInPlayer)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Cyan, TEXT("Player ENTERED"));

	PlayersInSession.Add(LogInPlayer);
	OnPlayersListRefreshed.Broadcast();
}

void UDB_OnlineSubSystem::RemovePlayerFromSession(APlayerController* LogInPlayer)
{
	GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Cyan, TEXT("Player EXITED"));

	if (PlayersInSession.Contains(LogInPlayer))
	{
		PlayersInSession.Remove(LogInPlayer);
		OnPlayersListRefreshed.Broadcast();
	}
}

void UDB_OnlineSubSystem::CreateSession()
{
	FString PlayerName = Cast<UDB_GameInstance>(World->GetGameInstance())->PlayerName;

	SessionName = FName(*FString::Printf(TEXT("Session_%s"), *PlayerName));

	UE_LOG(LogTemp, Warning, TEXT("[DB] Starting find session"));

	if (OnlineSub)
	{
		if (SessionInterface.IsValid()) 
		{
			if (IOnlineSubsystem::Get()->GetSubsystemName() == "NULL")
			{
				OnCreateSessionCompleteDelegate_Handle(SessionName, true);
				return;
			}


			auto ExistingSession = SessionInterface->GetNamedSession(SessionName);

			if (ExistingSession != nullptr)
			{
				IsRecreatingSession = true;
				SessionInterface->DestroySession(SessionName);
			}
			else
			{

				ServerName = FString("HOST PLAYER: ") + PlayerName;

				FOnlineSessionSettings* HostSettings = new FOnlineSessionSettings();

				HostSettings->bIsLANMatch = IOnlineSubsystem::Get()->GetSubsystemName() == "NULL";

				HostSettings->bAllowInvites = true;
				HostSettings->bAllowJoinInProgress = true;
				HostSettings->bIsDedicated = false;
				HostSettings->NumPublicConnections = 1000;
				HostSettings->bAllowJoinViaPresence = true;
				HostSettings->bShouldAdvertise = true;
				HostSettings->bUsesPresence = true; // this is needed so we can create lobby session instead of internet session (f12 on create session below to see usage)

				HostSettings->Set(SERVER_NAME_SETTINGS_KEY, ServerName, EOnlineDataAdvertisementType::ViaOnlineServiceAndPing);


				SessionInterface->CreateSession(0, SessionName, *HostSettings);
			}			
		}
	}	
}

void UDB_OnlineSubSystem::OnCreateSessionCompleteDelegate_Handle(FName SessionName, bool Success)
{
	FString SuccessStr = Success ? TEXT("SUCCESS") : TEXT("FAILED");
	UE_LOG(LogTemp, Warning, TEXT("[DB]  Session Name <%s> -> Success: %s"), *SessionName.ToString(), *SuccessStr);

	if (!Success)
		return;
	
	

	if (!ensure(World != nullptr))
	{
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Yellow, TEXT("NO WORLD WTF"));
		return;
	}

	World->ServerTravel("/Game/0Game/Maps/GameplayMap?listen");



	PlayersInSession.Empty();

}

void UDB_OnlineSubSystem::RefreshSessions()
{
	SessionSearch = MakeShareable(new FOnlineSessionSearch()); 

	if (SessionSearch.IsValid()) {
		SessionSearch->bIsLanQuery = IOnlineSubsystem::Get()->GetSubsystemName() == "NULL";
		SessionSearch->MaxSearchResults = 200000;
		SessionSearch->QuerySettings.Set(SEARCH_PRESENCE, true, EOnlineComparisonOp::Equals);

		// find the sessions
		SessionInterface->FindSessions(0, SessionSearch.ToSharedRef());
	}
}



void UDB_OnlineSubSystem::OnSessionsRefreshed_Handle(bool Success)
{
	if (!SessionSearch.IsValid() || !Success)
	{
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red,FString::Printf(TEXT("Invalid Session Search or NOT SUCCESS")));
		return;
	}

	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red,FString::Printf(TEXT("SERVERS FOUND--> %d"), SessionSearch->SearchResults.Num())	);

	ServersFound.Empty();

	int IndexID = 0;

	for (const FOnlineSessionSearchResult& SearchResult : SessionSearch->SearchResults)
	{
		FServerData ServerData;
		ServerData.Name = SearchResult.GetSessionIdStr();
		ServerData.MaxPlayers = SearchResult.Session.SessionSettings.NumPublicConnections;
		ServerData.CurrentPlayers = ServerData.MaxPlayers - SearchResult.Session.NumOpenPublicConnections;
		ServerData.HostUsername = SearchResult.Session.OwningUserName;
		ServerData.Index = IndexID;

		FString ServerName;
		if (SearchResult.Session.SessionSettings.Get(SERVER_NAME_SETTINGS_KEY, ServerName)) {
			ServerData.Name = ServerName;
		}
		else {
			ServerData.Name = "Could not find name";
		}

		ServersFound.Add(ServerData);
		IndexID++;
	}

	OnListRefreshed.Broadcast();
}




void UDB_OnlineSubSystem::JoinSession(FServerData SessionData)
{
	SessionInterface->JoinSession(0, FName(*SessionData.Name), SessionSearch->SearchResults[SessionData.Index]);
}

void UDB_OnlineSubSystem::OnJoinSessionComplete_Handle(FName SessionName, EOnJoinSessionCompleteResult::Type Result)
{
	if (!SessionInterface.IsValid())
	{
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("INVALID SESSION INTERFACE WHEN JOINING"));
		return;
	}

	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, FString::Printf(TEXT("JOINED TO: %s"), *SessionName.ToString()));	


	// get the connect string from the session interface and store platform specific connection information into Address
	FString Address;
	if (!SessionInterface->GetResolvedConnectString(SessionName, Address)) {
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("NO RESOLVED CONNEC STRING"));
		return;
	}

	UEngine* Engine = GEngine;
	if (!ensure(Engine != nullptr))
	{
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("NO ENGINE"));
		return;
	}


	APlayerController* PC = World->GetFirstPlayerController();
	if (!ensure(PC != nullptr))
	{
		GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("NO LOCAL PLAYERCONTROLLER"));
		return;
	}

	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, FString::Printf(TEXT("Travelling to: %s"), *Address));
	PC->ClientTravel(Address, ETravelType::TRAVEL_Absolute);

}


void UDB_OnlineSubSystem::DestroySession()
{
	IsRecreatingSession = false;

	if (SessionInterface)
	{
		SessionInterface->DestroySession(SessionName);
	}	
}





void UDB_OnlineSubSystem::OnStartSessionCompleteDelegate_Handle(FName SessionName, bool Success)
{
	for(FConstPlayerControllerIterator It = GetWorld()->GetPlayerControllerIterator(); It; ++It)
	{
		AUEMakeItCountPlayerController* PC = Cast< AUEMakeItCountPlayerController>(*It);
		if (PC && !PC->IsLocalPlayerController())
		{
			PC->ClientStartOnlineSession();
		}
	}
}



void UDB_OnlineSubSystem::OnDestroySessionComplete_Handle(FName SessionName, bool Success)
{
	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("Session Destroyed"));
	UE_LOG(LogTemp, Warning, TEXT("[DB] Session Destroyed"));

	if (IsRecreatingSession)
	{
		CreateSession();
	}
}
