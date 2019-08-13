// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "DB_Songs_DataAsset.generated.h"

/**
 * 
 */
UCLASS(BlueprintType, Blueprintable)
class UEMAKEITCOUNT_API UDB_Songs_DataAsset : public UDataAsset
{
	GENERATED_BODY()
	
public:

	UPROPERTY(EditAnywhere, BlueprintReadOnly)
	TArray<class USoundWave*> Songs;

	UFUNCTION(BlueprintCallable)
	USoundWave* GetRandomSong()
	{
		return Songs[FMath::RandRange(0, Songs.Num() - 1)];
	}

};
