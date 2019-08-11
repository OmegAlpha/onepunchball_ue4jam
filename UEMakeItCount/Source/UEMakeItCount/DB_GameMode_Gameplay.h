// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "DB_GameInfoActor.h"
#include "DB_GameMode_Gameplay.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API ADB_GameMode_Gameplay : public AGameModeBase
{
	GENERATED_BODY()

public:

	ADB_GameMode_Gameplay();

	virtual void BeginPlay() override;

	void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

 	UPROPERTY(Replicated, BlueprintReadOnly)
 	ADB_GameInfoActor* GameInfo;

	virtual void PostLogin(APlayerController* NewPlayer) override;

	virtual void Logout(AController* Exiting) override;


	void CreateGameInfo();

};
