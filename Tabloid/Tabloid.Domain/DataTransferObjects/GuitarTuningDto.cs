﻿namespace Tabloid.Domain.DataTransferObjects
{
    public class GuitarTuningDto
    {
        public Guid Id { get; set; }

        public int StringNumber { get; set; }

        public string Name { get; set; }

        public string Tuning { get; set; }
    }
}
