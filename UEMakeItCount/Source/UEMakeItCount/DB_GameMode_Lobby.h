// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "DB_GameMode_Lobby.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API ADB_GameMode_Lobby : public AGameModeBase
{
	GENERATED_BODY()

public:

	ADB_GameMode_Lobby();

	virtual void PostLogin(APlayerController* NewPlayer) override;

	virtual void Logout(AController* Exiting) override;
};
