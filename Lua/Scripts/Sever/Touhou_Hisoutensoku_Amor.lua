--[[
    This example adds a special type of armor that can absorb damage, and when it's condition reaches 0, it breaks.
--]]

local function Clamp(n, min, max)
    if n < min then
        n = min
    elseif n > max then
        n = max
    end

    return n
end

Hook.Add("character.applyDamage", "LuaArmor.ApplyDamage", function (characterHealth, attackResult, hitLimb)

    local character = characterHealth.Character
    if character.Inventory == nil then return end
    local armor = character.Inventory.GetItemInLimbSlot(InvSlotType.OuterClothes)

    if armor == nil or armor.Prefab.Identifier ~= "Touhou_Hisoutensoku" then return end

    local damage = 0
    local CharacterResistance  = Clamp(characterHealth.GetResistance(affliction.Prefab) , 0, 0.99)

    for affliction in attackResult.Afflictions do
        damage = damage + affliction.Strength
    end

    armor.Condition = armor.Condition - damage * (1 - CharacterResistance)

    if armor.Condition > 0 then
        return true
    else
        Entity.Spawner.AddEntityToRemoveQueue(armor)
    end
end)