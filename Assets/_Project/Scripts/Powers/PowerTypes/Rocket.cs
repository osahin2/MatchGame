using App;
using DG.Tweening;
using GridSystem;
using Item;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Power
{
    [Serializable]
    public class Rocket
    {
        [SerializeField] private Transform _self;
        [SerializeField] private List<FlyObject> _flyObjects = new();
        [SerializeField] private List<ParticleSystem> _particles = new();
        [SerializeField] private bool _isHorizontal;

        public void UseRocket(IGameBoard gameBoard, IGridSolver gridSolver, ItemMoveSettings moveSettings, Action onComplete)
        {
            var solvedGrids = new HashSet<IGridSlot>();
            var startSlot = gameBoard[gameBoard.WorldToGrid(_self.position)];
            solvedGrids.Add(startSlot);
            startSlot.Clear();
            PlayParticles();
            foreach (var item in _flyObjects)
            {
                item.flyObject.DOLocalMove(GetDirection(item) * moveSettings.RocketDistance, moveSettings.RocketMoveTime)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        var gridPos = gameBoard.WorldToGrid(item.flyObject.position);

                        if (!GridUtility.IsPositionOnGrid(gridPos, gameBoard.Width, gameBoard.Height))
                        {
                            item.isReachedEnd = true;
                            return;
                        }
                        var slot = gameBoard[gridPos];

                        if (solvedGrids.Contains(slot))
                            return;

                        solvedGrids.Add(slot);
                        gridSolver.SingleGridHit(slot);

                    }).OnComplete(() =>
                    {
                        onComplete?.Invoke();
                        StopParticles();
                        //foreach (var slot in solvedGrids)
                        //{
                        //    gridSolver.SolveColumn(slot);
                        //}
                        gridSolver.Solve(solvedGrids);
                        Reset();
                    });
            }

        }
        private void Reset()
        {
            foreach (var item in _flyObjects)
            {
                item.flyObject.localPosition = Vector3.zero;
                item.isReachedEnd = false;
            }
        }
        private void PlayParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Play();
            }
        }
        private void StopParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        private Vector3 GetDirection(FlyObject obj)
        {
            if (_isHorizontal)
            {
                return obj.flyObject.right * obj.direction;
            }
            return obj.flyObject.up * obj.direction;
        }

        [Serializable]
        private class FlyObject
        {
            public Transform flyObject;
            public float direction;
            public bool isReachedEnd;
        }
    }

}

