﻿using System;
using Tools.Input.Mouse;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace HexBoardGame.UI
{
    [RequireComponent(typeof(IMouseInput)), RequireComponent(typeof(Tilemap)),
     RequireComponent(typeof(TilemapCollider2D)), RequireComponent(typeof(UiBoard))]
    public class UiTileMapInputHandler : MonoBehaviour
    {
        Camera Camera { get; set; }
        Tilemap TileMap { get; set; }
        UiBoard UiBoard { get; set; }
        IMouseInput Input { get; set; }
        public event Action<Vector3Int> OnClickTile = cell => { };
        public event Action<Vector3Int, Vector2> OnRightClickTile = (cell, screenPoint) => { };

        void OnPointerClick(PointerEventData eventData)
        {
            var screenPosition = eventData.position;
            var cell = ConvertPixelToCell(screenPosition);
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnClickTile.Invoke(cell);
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightClickTile.Invoke(cell, screenPosition);
                    break;
            }
        }

        void Awake()
        {
            Camera = Camera.main;
            UiBoard = GetComponent<UiBoard>();
            TileMap = GetComponentInChildren<Tilemap>();
            Input = GetComponent<IMouseInput>();
            Input.OnPointerClick += OnPointerClick;
        }

        Vector3Int ConvertPixelToCell(Vector2 screenPoint)
        {
            var worldPosition = Camera.ScreenToWorldPoint(screenPoint);
            var cell = TileMap.WorldToCell(worldPosition);
            return cell;
        }
    }
}