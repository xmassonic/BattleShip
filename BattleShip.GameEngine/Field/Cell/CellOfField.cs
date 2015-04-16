﻿using System;
using System.Collections.Generic;

using BattleShip.GameEngine.Location;
using BattleShip.GameEngine.GameObject;
using BattleShip.GameEngine.Arsenal.Protection;
using BattleShip.GameEngine.Field.Cell.StatusCell;
using BattleShip.GameEngine.GameEventArgs;
using BattleShip.GameEngine.Arsenal.Gun;
using BattleShip.GameEngine.Arsenal.Flot;
using BattleShip.GameEngine.Field.Cell.StatusCell;


namespace BattleShip.GameEngine.Field.Cell
{
    public sealed class CellOfField : IEnumerable<Type>
    {
        Position _position;

        bool _wasAttacked = false;

        public Position Location
        {
            get
            {
                return _position;
            }
        }

        public bool WasAttacked
        {
            get
            {
                return this._wasAttacked;
            }
        }

        // клвтинка поля може містити обєкт поля(захист чи кораблик, і т.д)
        private GameObject.GameObject _gameObject;

        // клітинка поля може бути захищена декількома захистами(записуються їхні імена)
        List<Type> _protectionTypeList = new List<Type>();

        public CellOfField(Position position)
        {
            _position = position;
            _gameObject = new EmptyCell(position);
        }

        public void AddProtect(ProtectedBase protect)
        {
            _gameObject = protect;

            SetProtect(protect);

            // підписати захист на руйнацію під час руйнації клітинки
            DeadHandler += protect.OnHitMeHandler;
        }

        public void AddStatus(StatusCell.StatusCell status)
        {
            _gameObject = status;
            DeadHandler += status.OnHitMeHandler;
        }

        // встановити захист
        public void SetProtect(ProtectedBase protect)
        {
            _protectionTypeList.Add(protect.GetType());
        }

        public void OnRemoveProtection(GameObject.GameObject sender, ProtectEventArgs e)
        {
            _protectionTypeList.Remove(e.Type);
        }

        // отримати список захистів, які є на клітинці
        public IEnumerator<Type> GetEnumerator()
        {
            return _protectionTypeList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (IEnumerator<Type>)GetEnumerator();
        }


        public void AddShip(ShipRectangleBase ship)
        {
            _gameObject = ship;

            // підписати об'єкт на руйнацію під час руйнації клітинки
            DeadHandler += ship.OnHitMeHandler;
        }

        // івент знищення клітинки
        public event Action<GameObject.GameObject, GameEvenArgs> DeadHandler = delegate { };

        // пальнути в цю клітинку
        public Type Shot(Gun gun)
        {
            _wasAttacked = true;

            // якщо клітинка захищена, тоді повернути результат атаки - як захищену клітинку

            foreach (Type t in _protectionTypeList)
            {
                Type baseProtected = t;
                while (baseProtected != typeof())
                {
                    Type test = baseProtected;
                    if (test.BaseType == null)
                        break;
                    baseProtected = baseProtected.BaseType;
                }
            }

            if (_protectionTypeList.Contains(gun.GetCurrentCun()))
                return typeof(AttackResult.ProtectedCell);

            OnHitMeHandler(_gameObject, new GameEvenArgs(this._position));

            return _gameObject.GetType();
        }

        // опрацювання вмирання клітинки
        public void OnDeadHandler()
        {
            // сказати всім, хто на неї підписаний, що її зачепили
            DeadHandler(_gameObject, new GameEventArgs.GameEvenArgs(this._position));
        }

        public void OnHitMeHandler(GameObject.GameObject sender, GameEvenArgs e)
        {
            this._wasAttacked = true;
            OnDeadHandler();
        }

        // подивитись на цю клітинку
        public Type Show()
        {
            // якщо ще не була атаковано - то показати як пусту(невідому)
            if (!_wasAttacked)
                return typeof(EmptyCell);

            return _gameObject.GetType();
        }

        public Type GetStatusCell()
        {
            return _gameObject.GetType();
        }
       
    }
}