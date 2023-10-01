#nullable disable
using System;

namespace BUtil.Configurator.Controls
{
    class WhatItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public WhatItemType Type { get; set; }
    }
}
