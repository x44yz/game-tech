using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test1View : ViewBase<Test1ViewModel>
{
	public Text txt;

	// public Test1ViewModel ViewModel { get { return null;} }

	protected override void InitBinder()
	{
		base.InitBinder();
		Bind(viewModel.count, OnCountChanged);
	}

	void Update()
	{
		
	}

	public void OnClickBtnAdd()
	{
		Debug.Log("xx-- click btn add.");
		viewModel.count.Value += 1;
	}

	public void OnCountChanged(int oldValue, int newValue)
	{
		Debug.Log("xx-- OnCountChanged > " + oldValue + " - " + newValue);
		txt.text = newValue.ToString();
	}
}
