// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "UB_GameMode_MainMenu.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API AUB_GameMode_MainMenu : public AGameModeBase
{
	GENERATED_BODY()

public:

	AUB_GameMode_MainMenu();

	void BeginPlay() override;

	virtual void PostLogin(APlayerController* NewPlayer) override;

	virtual void Logout(AController* Exiting) override;
	
};
