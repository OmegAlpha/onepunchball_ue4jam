// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_PlayerCameraManager.h"

void ADB_PlayerCameraManager::DoUpdateCamera(float DeltaTime)
{
	Super::DoUpdateCamera(DeltaTime);


	const float PixelsPerUnits = 0.24f;
	const float UnitsPerPixel = 1.0f / PixelsPerUnits;

	FMinimalViewInfo CameraCachePOV = GetCameraCachePOV();
	CameraCachePOV.Location.X = FMath::GridSnap(CameraCachePOV.Location.X, UnitsPerPixel);
	CameraCachePOV.Location.Z = FMath::GridSnap(CameraCachePOV.Location.Z, UnitsPerPixel);
	FillCameraCache(CameraCachePOV);

}
