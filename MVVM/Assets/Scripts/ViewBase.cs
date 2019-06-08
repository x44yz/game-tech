using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase<T> : MonoBehaviour where T : ViewModelBase
{


	public void Bind<T>(Property<T> pp, Property<T>.ValueChangedHandler valueChangedHandler)
	{

	}

	public void Unbind<T>()
}
