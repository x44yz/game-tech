using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase<T> : MonoBehaviour where T : ViewModelBase, new()
{
	protected T _viewModel = null;
	public T viewModel {
		get {
			if (_viewModel == null)
				_viewModel = new T();
			return _viewModel;
		}
	}

	protected virtual void OnEnable()
	{
		
	}

	protected virtual void OnDisable()
	{
		
	}

	// TODO:
	// Bind 是否只能在 Start, OnEnable 里面调用，如果在 Awake 里面调用？
	public void Bind<PT>(Property<PT> pp, Property<PT>.ValueChangedHandler valueChangedHandler)
	{
		Debug.Assert(pp != null, "CHECK");
		Debug.Assert(valueChangedHandler != null, "CHECK");

		pp.OnValueChanged += valueChangedHandler;

		// TODO:
		// 是否第一次 bind 的时候直接调用回调
		pp.ValueChanged(pp.Value, pp.Value);
	}

	public void Unbind<PT>(Property<PT> pp, Property<PT>.ValueChangedHandler valueChangedHandler)
	{
		pp.OnValueChanged -= valueChangedHandler;
	}
}
