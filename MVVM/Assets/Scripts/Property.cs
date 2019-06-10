using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 对变量封装，获得变化的事件
public class Property<T>
{
	public delegate void ValueChangedHandler(T oldValue, T newValue);
	public ValueChangedHandler OnValueChanged;

	private T _value;
	public T Value
	{
		get { return _value; }
		set 
		{
			if (!Equals(_value, value))
			{
				T old = _value;
				_value = value;
				
				ValueChanged(old, _value);
			}
		}
	}

	public Property(T val)
	{
		Value = val;
	}

	public void ValueChanged(T oldValue, T newValue)
	{
		if (OnValueChanged != null)
			OnValueChanged(oldValue, newValue);
	}

	// public static Property<T> operator= (Property<T> p1, T val)
	// {
	// 	p1.Value = val;
	// 	return p1;
	// }
}
