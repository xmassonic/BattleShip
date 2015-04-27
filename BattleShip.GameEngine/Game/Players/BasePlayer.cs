﻿using BattleShip.GameEngine.Arsenal.Gun;
using System;
using System.Collections.Generic;
using BattleShip.GameEngine.Arsenal.Gun.Destroyable;

namespace BattleShip.GameEngine.Game.Players
{
    public abstract class BasePlayer : IPlayer
    {
        #region Members

        private string _name;

        public string Name
        {
            get { return _name; }
        }

        protected GameMode.GameMode gameMode;

        #endregion Members

        #region Constructor

        public BasePlayer(string name, GameMode.GameMode gameMode)
        {
            _name = name;
            this.gameMode = gameMode;
        }

        #endregion Constructor

        #region IPlayer realization

        public Field.FakeField CurrentFakeField
        {
            get
            {
                return gameMode.CurrentFakeField;
            }
        }

        public Field.Field CurreField
        {
            get
            {
                return gameMode.CurrentField;
            }
        }

        public List<IDestroyable> GunTypeList
        {
            get { return gameMode.GunTypesList; }
        }

        public Gun CurrentGun
        {
            get { return this.gameMode.CurrentGun; }
        }

        public List<Type> AttackMe(Arsenal.Gun.Gun gun, Location.Position position)
        {
            return this.gameMode.CurrentField.Shot(gun, position);
        }

        #endregion IPlayer realization
    }
}