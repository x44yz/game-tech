using System;
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

	public virtual void Show(Action onAfterShow = null)
	{
		// TODO:
		// 中间可以添加过渡动画
		OnBeforeShow();
		DoShow();
		// TODO:
		// 这里可能有打开过渡动画，不能直接调用 OnAfterShow()
		OnAfterShow();
	}

	public virtual void Hide(Action onAfterHide = null)
	{
		OnBeforeHide();
		DoHide();
		OnAfterHide();
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

	protected void OnBeforeShow()
	{
	}

	protected void DoShow()
	{
		Vector3 tpos = transform.position;
		tpos.x -= Screen.width * 2;
		transform.position = tpos;
	}

	protected void OnAfterShow()
	{
	}

	protected void OnBeforeHide()
	{
	}

	protected void DoHide()
	{
		Vector3 tpos = transform.position;
		tpos.x += Screen.width * 2;
		transform.position = tpos;
	}

	protected void OnAfterHide()
	{
	}
}
