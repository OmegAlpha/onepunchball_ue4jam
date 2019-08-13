// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "DB_GameConfig.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API UDB_GameConfig : public UPrimaryDataAsset
{
	GENERATED_BODY()
	
public:

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
	int KillsToWin;

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
	int SecondsToRound;

};
