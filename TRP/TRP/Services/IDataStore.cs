﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TRP.Models;

namespace TRP.Services
{
    public interface IDataStore
    {
        // Item
        Task<bool> InsertUpdateAsync_Item(Item data);
        Task<bool> AddAsync_Item(Item data);
        Task<bool> UpdateAsync_Item(Item data);
        Task<bool> DeleteAsync_Item(Item data);
        Task<Item> GetAsync_Item(string id);
        Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false);

        // Monster
        Task<bool> AddAsync_Monster(Monster data);
        Task<bool> UpdateAsync_Monster(Monster data);
        Task<bool> DeleteAsync_Monster(Monster data);
        Task<Monster> GetAsync_Monster(string id);
        Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false);

        // Character
        Task<bool> InsertUpdateAsync_Character(Character data);
        Task<bool> AddAsync_Character(Character data);
        Task<bool> UpdateAsync_Character(Character data);
        Task<bool> DeleteAsync_Character(Character data);
        Task<Character> GetAsync_Character(string id);
        Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false);

        // Score
        Task<bool> InsertUpdateAsync_Score(Score data);
        Task<bool> AddAsync_Score(Score data);
        Task<bool> UpdateAsync_Score(Score data);
        Task<bool> DeleteAsync_Score(Score data);
        Task<Score> GetAsync_Score(string id);
        Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false);
    }
}
