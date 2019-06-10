using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ViewModel 是数据抽象层
// 访问数据的来源，同时作为数据修改层和零时数据层
public class Test1ViewModel : ViewModelBase
{
	public Property<int> count = new Property<int>(0);
}
