// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/GameInstance.h"
#include "DB_GameInstance.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API UDB_GameInstance : public UGameInstance
{
	GENERATED_BODY()
	
public:

	UPROPERTY(BlueprintReadWrite)
	FString IndexSetup_HeadMesh;

	UPROPERTY(BlueprintReadWrite)
	FString IndexSetup_HeadColor;

	UPROPERTY(BlueprintReadWrite)
	FString IndexSetup_BodyMesh;

	UPROPERTY(BlueprintReadWrite)
	FString IndexSetup_BodyColor;

	UPROPERTY(BlueprintReadWrite)
	FString IndexSetup_WholeBodyColor;


	UPROPERTY(BlueprintReadWrite)
	class UUserWidget* MainParentWidget;

	UPROPERTY(BlueprintReadOnly)
	class UDB_OnlineSubSystem* OnlineSubsystem;

	UPROPERTY(BlueprintReadWrite)
	FString PlayerName = TEXT("Placehoder");

	virtual void Init() override;
};
