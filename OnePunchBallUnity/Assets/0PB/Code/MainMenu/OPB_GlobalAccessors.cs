using System;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class OPB_GlobalAccessors
{
    private static Dictionary<Type, List<EntityBehaviour>> spawnedEntitiesByBehType = new Dictionary<Type, List<EntityBehaviour>>();

    public static void AddEntity(Type type, EntityBehaviour entityBeh)
    {
        if (!spawnedEntitiesByBehType.ContainsKey(type))
            spawnedEntitiesByBehType.Add(type, new List<EntityBehaviour>());

        spawnedEntitiesByBehType[type].Add(entityBeh);
    }
    
    public static void RemoveEntity(Type type, EntityBehaviour entityBeh)
    {
        if (!spawnedEntitiesByBehType.ContainsKey(type))
            return;

        spawnedEntitiesByBehType[type].Remove(entityBeh);

    }


    public static List<EntityBehaviour> GetAllEntitiesOfType(Type entityType)
    {
        if (!spawnedEntitiesByBehType.ContainsKey(entityType))
            return new List<EntityBehaviour>();

        return spawnedEntitiesByBehType[entityType];
    }
}