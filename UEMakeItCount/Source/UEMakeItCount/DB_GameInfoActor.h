// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "GameFramework/PlayerController.h"
#include "DB_GameInfoActor.generated.h"

UCLASS(Blueprintable, BlueprintType)
class UEMAKEITCOUNT_API ADB_GameInfoActor : public AActor
{
	GENERATED_BODY()
	
public:	
	
	ADB_GameInfoActor();

	UPROPERTY(Replicated, BlueprintReadOnly)
	TArray<APlayerController*> PlayerInMatch;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	UFUNCTION(Server, Reliable, WithValidation)
	void AddPlayerToSession(APlayerController* Controller);

	void AddPlayerToSession_Implementation(class APlayerController* Controller);
	bool AddPlayerToSession_Validate(class APlayerController* Controller);

	UFUNCTION()
	void RemovePlayerToSession(APlayerController* Controller);


};

