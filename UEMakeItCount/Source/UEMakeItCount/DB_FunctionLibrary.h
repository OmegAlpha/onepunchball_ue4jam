// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "DB_FunctionLibrary.generated.h"

/**
 * 
 */
UCLASS()
class UEMAKEITCOUNT_API UDB_FunctionLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:
	
	UFUNCTION(BlueprintPure, Category = "Global")
	static UDB_OnlineSubSystem* GetOnline(UObject* WorldContext);


	UFUNCTION(BlueprintPure, Category = "Global")
	static bool CheckUserName(FString UserName);

};
