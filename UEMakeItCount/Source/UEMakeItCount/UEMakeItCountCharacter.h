// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "UnrealNetwork.h"
#include "CoreNet.h"
#include "UEMakeItCountCharacter.generated.h"


UCLASS(Blueprintable)
class AUEMakeItCountCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	AUEMakeItCountCharacter();


	void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

	UPROPERTY(Replicated, BlueprintReadWrite)
	FString Index_HeadMesh = TEXT("0");

	UPROPERTY(Replicated, BlueprintReadWrite)
	FString PlayerName;

	UPROPERTY(Replicated, BlueprintReadWrite)
	int QtyKills;

	// Called every frame.
	virtual void Tick(float DeltaSeconds) override;

	/** Returns TopDownCameraComponent subobject **/
	FORCEINLINE class UCameraComponent* GetTopDownCameraComponent() const { return TopDownCameraComponent; }
	/** Returns CameraBoom subobject **/
	FORCEINLINE class USpringArmComponent* GetCameraBoom() const { return CameraBoom; }
	/** Returns CursorToWorld subobject **/
	FORCEINLINE class UDecalComponent* GetCursorToWorld() { return CursorToWorld; }

	UFUNCTION(BlueprintImplementableEvent)
	void OnShootDone(FVector TargetDirection);
	
	UFUNCTION(Server, BlueprintCallable, Reliable, WithValidation)
	void RegisterKill(int Qty);

	void RegisterKill_Implementation(int Qty);
	bool RegisterKill_Validate(int Qty);

	
	void MoveForward(float Value);
	void MoveRight(float Value);

protected:

	void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	void BeginPlay() override;




private:

	

	/** Top down camera */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
	class UCameraComponent* TopDownCameraComponent;

	/** Camera boom positioning the camera above the character */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
	class USpringArmComponent* CameraBoom;

	/** A decal that projects to the cursor location. */
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
	class UDecalComponent* CursorToWorld;

	

};

