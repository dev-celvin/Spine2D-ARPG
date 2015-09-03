using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{
	
	private static T m_Instance = null;
	
	public static T instance{
		get{
			if( m_Instance == null ){
				m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
				if( m_Instance == null ){
					//这里可以看到生成的Singleton对象是在Hierarchy中以Singleton of 单例类名 形式存在的，单例过多会导致Hierarchy看着混乱，可以整理到一个对象下去
					m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
					//在创建的时候这种情况一般不会发生
					if( m_Instance == null ) {
						Debug.LogError("Problem during the creation of " + typeof(T).ToString());
					}
					m_Instance.Init();
					
				}
				
			}
			return m_Instance;
		}
	}
	private void Awake(){
		
		if( m_Instance == null ){
			m_Instance = this as T;
			m_Instance.Init();
		}
	}
	//该函数用来初始化一些数据
	public virtual void Init(){}
	
	//确保在程序退出时销毁实例。
	private void OnApplicationQuit(){
		m_Instance = null;
	}
}
