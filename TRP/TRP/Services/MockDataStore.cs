﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRP.Models;
using TRP.ViewModels;

namespace TRP.Services
{
    public sealed class MockDataStore : IDataStore
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static MockDataStore _instance;

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
        public static MockDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MockDataStore();
                }
                return _instance;
            }
        }

        private List<Item> _itemDataset = new List<Item>(); // Items dataset
        private List<Character> _characterDataset = new List<Character>(); // Characters dataset
        private List<Monster> _monsterDataset = new List<Monster>(); // Monsters dataset
        private List<Score> _scoreDataset = new List<Score>(); // Scores dataset

        // Constructor: adds data to dataset
        private MockDataStore()
        {
            InitilizeSeedData();
        }

        // Adds data
        private void InitilizeSeedData()
        {
            // Load items
            _itemDataset.Add(new Item("Slush Helmet", "A helmet made from slush",
                "https://www.iconsdb.com/icons/preview/caribbean-blue/helmet-xxl.png", 0, 1, 0, ItemLocationEnum.Head, AttributeEnum.Defense));
            _itemDataset.Add(new Item("Ice Boots", "Boots with harden iced attached",
                "https://vikings.help/users/vikings/imgExtCatalog/big/m321.png", 0, 3, 0, ItemLocationEnum.Feet, AttributeEnum.Defense));
            _itemDataset.Add(new Item("Fire Bow", "Crafted from artic flames",
                "https://vignette.wikia.nocookie.net/callofduty/images/5/54/Kreeaho%27ahm_nal_Ahmhogaroc_third_person_BO3_Transparent.png", 
                4, 3, 3, ItemLocationEnum.PrimaryHand, AttributeEnum.Attack));


            // Load characters
            _characterDataset.Add(new Character("Poppy", new AttributeBase(10, 4, 4, 2), PenguinTypeEnum.Emperor));
            _characterDataset.Add(new Character("Perry", new AttributeBase(10, 4, 2, 4), PenguinTypeEnum.Little));
            _characterDataset.Add(new Character("Paco", new AttributeBase(10, 3, 3, 4), PenguinTypeEnum.Gentoo));
            _characterDataset.Add(new Character("Patrick", new AttributeBase(10, 3, 4, 3), PenguinTypeEnum.Macaroni));
            _characterDataset.Add(new Character("Pennie", new AttributeBase(10, 4, 2, 4), PenguinTypeEnum.Adelie));
            _characterDataset.Add(new Character("Percy", new AttributeBase(10, 4, 3, 3), PenguinTypeEnum.Magellanic));
            _characterDataset.Add(new Character("Patty", new AttributeBase(10, 4, 4, 2), PenguinTypeEnum.Rockhopper));
            _characterDataset.Add(new Character("Penelope", new AttributeBase(10, 3, 5, 2), PenguinTypeEnum.King));

            // Load monsters 
            _monsterDataset.Add(new Monster("Leonard", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.LeopardSeal));
            _monsterDataset.Add(new Monster("Arnie", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.Fox));
            _monsterDataset.Add(new Monster("Oscar", new AttributeBase(15, 3, 3, 1), MonsterTypeEnum.Orca));
            _monsterDataset.Add(new Monster("Sally", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.SeaLion));
            _monsterDataset.Add(new Monster("Philip", new AttributeBase(10, 2, 2, 1), MonsterTypeEnum.PolarBear));
            _monsterDataset.Add(new Monster("Scott", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.SeaEagle));
            _monsterDataset.Add(new Monster("Sue", new AttributeBase(5, 1, 1, 1), MonsterTypeEnum.Skua));
            _monsterDataset.Add(new Monster("Saul", new AttributeBase(10, 2, 2, 1), MonsterTypeEnum.Shark));
           
            // Load Scores
            var mockScores = new List<Score>
            {
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockFirst Score", ScoreTotal = 111},
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockSecond Score", ScoreTotal = 222},
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockThird Score", ScoreTotal = 333},
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockFourth Score", ScoreTotal = 444},
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockFifth Score", ScoreTotal = 555},
                new Score { Id = Guid.NewGuid().ToString(), Name = "MockSixth Score", ScoreTotal = 666},
            };

            foreach (var data in mockScores)
            {
                _scoreDataset.Add(data);
            }
        }

        private void CreateTables()
        {
            // Do nothing...
        }

        // Delete the Database Tables by dropping them
        public void DeleteTables()
        {
            // Implement
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            // Implement Scores
        }

        // Refreshes database 
        public void InitializeDatabaseNewTables()
        {
            // Delete tables
            DeleteTables();

            // make them again
            CreateTables();

            // Populate them
            InitilizeSeedData();

            // Tell View Models they need to refresh
            NotifyViewModelsOfDataChange();
        }

        #region Item
        // Item
        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                _itemDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                await AddAsync_Item(data);
                return true;
            }

            return false;
        }

        // Add item to mock datastore 
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Update item in mock datastore
        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete item in mock datastore
        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get item from mock datastore
        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Id == id));
        }

        // Get list of items from mock datastore
        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            return await Task.FromResult(_itemDataset);
        }

        #endregion Item

        #region Character
        // Character
        public async Task<bool> InsertUpdateAsync_Character(Character data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                _characterDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Character(data);
            if (UpdateResult)
            {
                await AddAsync_Character(data);
                return true;
            }

            return false;
        }

        // Adds character to the dataset
        public async Task<bool> AddAsync_Character(Character data)
        {
            data.Update(data);
            _characterDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Finds character in dataset and updates it
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Removes character from dataset
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _characterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Retrieves matching character from dataset
        public async Task<Character> GetAsync_Character(string id)
        {
            return await Task.FromResult(_characterDataset.FirstOrDefault(s => s.Id == id));
        }

        // Retrieves all characters from dataset
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            return await Task.FromResult(_characterDataset);
        }

        #endregion Character

        #region Monster
        //Monster

        // Add monster to mock datastore
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            // Update the monster data
            data.Update(data);

            // Add to dataset
            _monsterDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Update the monster in the mock datastore
        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            // Get the monster based on id
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);

            // Check that the data is not null
            if (myData == null)
            {
                return false;
            }

            // Update the monster with data
            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete the monster from the mock datastore
        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            // Get the monster based on id
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);

            // Check that the data is not null
            if (myData == null)
            {
                return false;
            }

            // Remove the monster from the datastore
            _monsterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get the monster from the mock datastore
        public async Task<Monster> GetAsync_Monster(string id)
        {
            return await Task.FromResult(_monsterDataset.FirstOrDefault(s => s.Id == id));
        }

        // Get a list of monsters from the mock datastore
        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            return await Task.FromResult(_monsterDataset);
        }

        #endregion Monster

        #region Score
        // Score
        public async Task<bool> InsertUpdateAsync_Score(Score data)
        {
            // Check to see if the score exist
            var oldData = await GetAsync_Score(data.Id);
            if (oldData == null)
            {
                _scoreDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Score(data);
            if (UpdateResult)
            {
                await AddAsync_Score(data);
                return true;
            }
            return false;
        }

        // Add a score to the mock datastore
        public async Task<bool> AddAsync_Score(Score data)
        {
            // Update the score data
            data.Update(data);

            // Add to dataset
            _scoreDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Update a score in the mock datastore
        public async Task<bool> UpdateAsync_Score(Score data)
        {
            // Get the score based on id
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);

            // Check that the data is not null
            if (myData == null)
            {
                return false;
            }

            // Update the score with new data
            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete a score from the mock datastore
        public async Task<bool> DeleteAsync_Score(Score data)
        {
            // Get the score based on id
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);

            // Check that the data is not null
            if (myData == null)
            {
                return false;
            }

            // Remove the score from the datastore
            _scoreDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get a score from the mock datastore
        public async Task<Score> GetAsync_Score(string id)
        {
            return await Task.FromResult(_scoreDataset.FirstOrDefault(s => s.Id == id));
        }

        // Get a list of scores from the mock datastore
        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            return await Task.FromResult(_scoreDataset);
        }
        #endregion Score
    }
}