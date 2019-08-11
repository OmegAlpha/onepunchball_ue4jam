// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_KillerBall.h"
#include "Components/SceneComponent.h"
#include "Components/PrimitiveComponent.h"
#include "Engine/Engine.h"
#include "Components/SphereComponent.h"

// Sets default values
ADB_KillerBall::ADB_KillerBall()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("BallMesh"));
	Mesh->SetupAttachment(GetRootComponent());

	SphereCollision = CreateDefaultSubobject<USphereComponent>(TEXT("SphereCollision"));
	SphereCollision->SetupAttachment(Mesh);

}

// Called when the game starts or when spawned
void ADB_KillerBall::BeginPlay()
{
	Super::BeginPlay();
	
	SphereCollision->OnComponentBeginOverlap.AddDynamic(this, &ADB_KillerBall::OnBeginOverlap);

//	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("begin lalalalal"));
}

void ADB_KillerBall::OnBeginOverlap(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
//	GEngine->AddOnScreenDebugMessage(-1, 3.f, FColor::Red, TEXT("OVERLAP ASDFASDFASFDASDF"));

	DoOnOverlapSomething(OtherActor, SweepResult);
}

// Called every frame
void ADB_KillerBall::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

