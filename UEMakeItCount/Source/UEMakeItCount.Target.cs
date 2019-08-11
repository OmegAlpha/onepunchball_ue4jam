// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.Collections.Generic;

public class UEMakeItCountTarget : TargetRules
{
	public UEMakeItCountTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Game;
		ExtraModuleNames.Add("UEMakeItCount");

        bUsesSteam = true;
    }
}
