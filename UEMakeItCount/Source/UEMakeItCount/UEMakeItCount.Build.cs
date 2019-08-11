// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class UEMakeItCount : ModuleRules
{
	public UEMakeItCount(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(new string[] {
            "Core", "CoreUObject", "Engine", "InputCore", "HeadMountedDisplay", "NavigationSystem", "AIModule",
            "UMG", "Slate", "SlateCore",

            "OnlineSubsystem",
            "OnlineSubsystemUtils"

        });

        DynamicallyLoadedModuleNames.Add("OnlineSubsystemSteam");
    }
}
