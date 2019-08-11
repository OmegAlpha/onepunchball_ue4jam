// Fill out your copyright notice in the Description page of Project Settings.

#pragma once


#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "OnlineSubsystem.h"
#include "OnlineSessionInterface.h"
#include "IDelegateInstance.h"
#include "DB_OnlineSubSystem.generated.h"

USTRUCT(BlueprintType)
struct FServerData {
	GENERATED_BODY()

	UPROPERTY(BlueprintReadOnly)
	int32 Index;

	UPROPERTY(BlueprintReadOnly)
	FString Name;

	UPROPERTY(BlueprintReadOnly)
	int32 CurrentPlayers;

	UPROPERTY(BlueprintReadOnly)
	int32 MaxPlayers;

	UPROPERTY(BlueprintReadOnly)
	FString HostUsername;
};

UCLASS()
class UEMAKEITCOUNT_API UDB_OnlineSubSystem : public UObject
{
	GENERATED_BODY()

public:	

	DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnListRefreshed_Delegate);
	DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnPlayersListRefreshed_Delegate);



	UPROPERTY(BlueprintAssignable)
	FOnListRefreshed_Delegate OnListRefreshed;

	UPROPERTY(BlueprintAssignable)
	FOnPlayersListRefreshed_Delegate OnPlayersListRefreshed;

	UPROPERTY(BlueprintReadOnly)
	TArray<APlayerController*> PlayersInSession;

	UPROPERTY()
	UWorld* World;

	UPROPERTY()
	bool IsRecreatingSession = false;

	IOnlineSubsystem* OnlineSub;
	IOnlineSessionPtr SessionInterface;
	TSharedPtr<class FOnlineSessionSearch> SessionSearch;
	FName SessionName; 

	UPROPERTY(BlueprintReadOnly)
	TArray<FServerData> ServersFound;

	UPROPERTY(BlueprintReadWrite)
	FString ServerName;

	UDB_OnlineSubSystem();

	void Init();

	UFUNCTION()
	void AddPlayerToSession(APlayerController* LogInPlayer);

	UFUNCTION()
	void RemovePlayerFromSession(APlayerController* LogInPlayer);


	UFUNCTION(BlueprintCallable)
	void CreateSession();

	UFUNCTION(BlueprintCallable)
	void JoinSession(FServerData SessionData);


	UFUNCTION(BlueprintCallable)
	void DestroySession();

	UFUNCTION(BlueprintCallable)
	void RefreshSessions();

	void OnCreateSessionCompleteDelegate_Handle(FName SessionName, bool Success);
	void OnStartSessionCompleteDelegate_Handle(FName SessionName, bool Success);
	void OnSessionsRefreshed_Handle(bool Success);
	void OnJoinSessionComplete_Handle(FName SessionName, EOnJoinSessionCompleteResult::Type Result);
	void OnDestroySessionComplete_Handle(FName SessionName, bool Success);
};
