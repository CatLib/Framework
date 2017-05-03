﻿using System.Collections.Generic;
using UnityEngine;
using FairyGUI.Utils;

namespace FairyGUI
{
	/// <summary>
	/// Gear is a connection between object and controller.
	/// </summary>
	public class GearIcon : GearBase
	{
		Dictionary<string, string> _storage;
		string _default;

		public GearIcon(GObject owner)
			: base(owner)
		{
		}

		protected override void Init()
		{
			_default = _owner.icon;
			_storage = new Dictionary<string, string>();
		}

		override protected void AddStatus(string pageId, string value)
		{
			if (pageId == null)
				_default = value;
			else
				_storage[pageId] = value;
		}

		override public void Apply()
		{
			_owner._gearLocked = true;

			string cv;
			if (!_storage.TryGetValue(_controller.selectedPageId, out cv))
				cv = _default;

			_owner.icon = cv;

			_owner._gearLocked = false;
		}

		override public void UpdateState()
		{
			_storage[_controller.selectedPageId] = _owner.icon;
		}
	}
}
