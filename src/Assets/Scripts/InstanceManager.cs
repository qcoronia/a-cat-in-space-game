using System;
using System.Linq;
using UnityEngine;

public static class InstanceManager
{
    private static GameManager _game;
    public static GameManager Game
    {
        get
        {
            _game = _game ?? GameObject.FindObjectOfType<GameManager>();
            return _game;
        }
    }

    private static ViewManager _view;
    public static ViewManager View
    {
        get
        {
            _view = _view ?? GameObject.FindObjectOfType<ViewManager>();
            return _view;
        }
    }

    private static CameraController _camera;
    public static CameraController Camera
    {
        get
        {
            _camera = _camera ?? GameObject.FindObjectOfType<CameraController>();
            return _camera;
        }
    }

    private static Spawner _rocks;
    public static Spawner Rocks
    {
        get
        {
            _rocks = _rocks ?? GameObject.FindObjectOfType<Spawner>();
            return _rocks;
        }
    }

    private static InputManager _input;
    public static InputManager Input
    {
        get
        {
            _input = _input ?? GameObject.FindObjectOfType<InputManager>();
            return _input;
        }
    }
}
