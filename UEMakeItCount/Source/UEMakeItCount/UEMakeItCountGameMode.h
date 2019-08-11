// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "UEMakeItCountGameMode.generated.h"

UCLASS(minimalapi)
class AUEMakeItCountGameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	AUEMakeItCountGameMode();

protected:

	void BeginPlay() override;

	void Tick(float DeltaTime) override;
};



