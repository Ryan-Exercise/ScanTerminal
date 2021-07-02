//  IBulkPriceHolder.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       20:2:4 1/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
namespace ScanTerminal
{
    public interface IBulkPriceHolder
    {
        int GetBulkCount();
        decimal GetBulkPrice();
    }
}
