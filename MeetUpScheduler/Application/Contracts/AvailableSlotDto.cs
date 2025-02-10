﻿using System.Text.Json.Serialization;

namespace Application.Contracts;

public class AvailableSlotDto
{
	[JsonPropertyName("available_count")]
	public int AvailableCount { get; set; }

	[JsonPropertyName("start_date")]
	public DateTimeOffset StartDate { get; set; }
}