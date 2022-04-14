﻿namespace Uno.Extensions.Serialization;

public class SystemTextJsonGeneratedSerializer<T> : ISerializer<T>, IStreamSerializer<T>
{
	public SystemTextJsonGeneratedSerializer(
		ISerializer nonTypedSerializer,
		IStreamSerializer nonTypedStreamSerializer,
		JsonTypeInfo<T>? typeInfo = null)
	{
		_nonTypedSerializer = nonTypedSerializer;
		_nonTypedStreamSerializer = nonTypedStreamSerializer;
		_typeInfo = typeInfo;
	}

	public string ToString(T value)
	{
		if (_typeInfo is not null)
		{
			return JsonSerializer.Serialize(value, _typeInfo);
		}

		return _nonTypedSerializer.ToString(value);
	}
	public T? FromString(string source)
	{
		if (_typeInfo is not null)
		{
			return JsonSerializer.Deserialize(source, _typeInfo);
		}

		return _nonTypedSerializer.FromString<T>(source);
	}
	public string ToString(object value, Type valueType) => valueType == typeof(T) ? ToString((T)value) : _nonTypedSerializer.ToString(value, valueType);
	public object? FromString(string source, Type targetType) => targetType == typeof(T) ? FromString(source) : _nonTypedSerializer.FromString(source, targetType);
	public T? FromStream(Stream source)
	{
		if (_typeInfo is not null)
		{
			return JsonSerializer.Deserialize(source, _typeInfo);
		}

		return _nonTypedStreamSerializer.FromStream<T>(source);
	}
	public void ToStream(Stream stream, T value)
	{
		if (_typeInfo is not null)
		{
			JsonSerializer.Serialize(stream, value, _typeInfo);
			return;
		}

		_nonTypedStreamSerializer.ToStream<T>(stream, value);
	}
	public object? FromStream(Stream source, Type targetType) => targetType == typeof(T) ? FromStream(source) : _nonTypedStreamSerializer.FromStream(source, targetType);
	public void ToStream(Stream stream, object value, Type valueType)
	{
		if (valueType == typeof(T))
		{

			ToStream(stream, (T)value);
		}
		else
		{
			_nonTypedStreamSerializer.ToStream(stream, value, valueType);
		}
	}

	private readonly ISerializer _nonTypedSerializer;
	private readonly IStreamSerializer _nonTypedStreamSerializer;
	private readonly JsonTypeInfo<T>? _typeInfo;
}