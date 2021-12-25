﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/ItemHandler/Weapon", fileName = "WeaponHandler")]
public class WeaponHandler : ItemHandler
{
    public List<int> damageByLevel;
    public int damage;

    [Header("Position Setup")] 
    public Vector3 initPosition;
    public Vector3 initRotation;
    
    public override bool OnEquipItem(BaseBlock block)
    {
        PlayerGear gear = Player.Instance.gears;
        
        if (!gear.IsEquipWeapon)
        {
            damage = damageByLevel[block.blockData.BlockLevel - 1];
            Sprite spr = block.blockItem.GetItemSprite();
            gear.EquipWeapon(this,spr);
            ItemEquipSlot.Equip(block.blockItem.config.itemType,block.blockItem.GetItemSprite());
            return true;
        }
        return false;
    }

    public WeaponHandler Clone()
    {
        return (WeaponHandler)MemberwiseClone();
    }
    
    public static WeaponHandler CreateInstance(WeaponHandler copier)
    {
        var data = ScriptableObject.CreateInstance<WeaponHandler>();
        data = copier.Clone();
        return data;
    }
}