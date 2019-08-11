// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#include "UEMakeItCountPlayerController.h"
#include "Blueprint/AIBlueprintHelperLibrary.h"
#include "Runtime/Engine/Classes/Components/DecalComponent.h"
#include "HeadMountedDisplayFunctionLibrary.h"
#include "UEMakeItCountCharacter.h"
#include "Engine/World.h"
#include "Kismet/GameplayStatics.h"
#include "DB_GameInfoActor.h"

AUEMakeItCountPlayerController::AUEMakeItCountPlayerController()
{
	bShowMouseCursor = true;
	DefaultMouseCursor = EMouseCursor::Crosshairs;
}

AUEMakeItCountCharacter* AUEMakeItCountPlayerController::GetGameCharacter()
{
	return Cast<AUEMakeItCountCharacter>(GetCharacter());
}

void AUEMakeItCountPlayerController::BeginPlay()
{
	Super::BeginPlay();

	

	if (HasAuthority() && IsLocalController())
	{		
		TArray<AActor*> FoundActors;
		UGameplayStatics::GetAllActorsOfClass(GetWorld(), ADB_GameInfoActor::StaticClass(), FoundActors);

		if (FoundActors.Num() > 0)
		{
			Cast< ADB_GameInfoActor>(FoundActors[0])->AddPlayerToSession(this);
		}		
	}
}

void AUEMakeItCountPlayerController::PlayerTick(float DeltaTime)
{
	Super::PlayerTick(DeltaTime);

}

void AUEMakeItCountPlayerController::SetupInputComponent()
{
	// set up gameplay key bindings
	Super::SetupInputComponent();

	InputComponent->BindAction("SetDestination", IE_Pressed, this, &AUEMakeItCountPlayerController::OnSetDestinationPressed);
	InputComponent->BindAction("SetDestination", IE_Released, this, &AUEMakeItCountPlayerController::OnSetDestinationReleased);

	// support touch devices 
	InputComponent->BindTouch(EInputEvent::IE_Pressed, this, &AUEMakeItCountPlayerController::MoveToTouchLocation);
	InputComponent->BindTouch(EInputEvent::IE_Repeat, this, &AUEMakeItCountPlayerController::MoveToTouchLocation);

	InputComponent->BindAction("ResetVR", IE_Pressed, this, &AUEMakeItCountPlayerController::OnResetVR);
}

void AUEMakeItCountPlayerController::OnResetVR()
{
	UHeadMountedDisplayFunctionLibrary::ResetOrientationAndPosition();
}

void AUEMakeItCountPlayerController::MoveToMouseCursor()
{
	
}

void AUEMakeItCountPlayerController::MoveToTouchLocation(const ETouchIndex::Type FingerIndex, const FVector Location)
{
	FVector2D ScreenSpaceLocation(Location);

	// Trace to see what is under the touch location
	FHitResult HitResult;
	GetHitResultAtScreenPosition(ScreenSpaceLocation, CurrentClickTraceChannel, true, HitResult);
	if (HitResult.bBlockingHit)
	{
		// We hit something, move there
		SetNewMoveDestination(HitResult.ImpactPoint);
	}
}

void AUEMakeItCountPlayerController::SetNewMoveDestination(const FVector DestLocation)
{
	
	
}

void AUEMakeItCountPlayerController::OnSetDestinationPressed()
{

	FHitResult Hit;
	GetHitResultUnderCursor(ECC_Visibility, false, Hit);

	if (Hit.bBlockingHit)
	{
		APawn* const MyPawn = GetPawn();
		if (MyPawn)
		{
			Cast<AUEMakeItCountCharacter>(MyPawn)->OnShootDone(Hit.ImpactPoint);
		}
	}
	
}

void AUEMakeItCountPlayerController::OnSetDestinationReleased()
{
	
}
