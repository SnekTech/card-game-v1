using System;
using System.Collections.Generic;
using Godot;

namespace CardGameV1;

public static class SceneFactory
{
    private static readonly Dictionary<Type, string> Paths = [];

    public static void Register<T>(string path) => Paths[typeof(T)] = path;

    public static T Instantiate<T>() where T : Node =>
        ResourceLoader.Load<PackedScene>(Paths[typeof(T)]).Instantiate<T>();
}