﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
    public interface IDataGet<T>
    {
        Task<T> GetItemAsync(string id);
        Task<List<T>> GetItemsAsync(bool forceRefresh = false);
        Task<List<T>> GetItemChildrenAsync(string id);
    }
}
