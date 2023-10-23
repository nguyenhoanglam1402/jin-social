using System;
using UnityEngine;

public class ResidentIdentification
{
	private string id;
	private string name;
	private int age;
	private GameObject characterObject;

	public string ID
	{
		get => id;
	}

	public string Name
	{
		get => name;
	}

	public int Age
	{
		get => age;
	}

	public ResidentIdentification(string id, string name, int age, GameObject characterObject)
	{
		this.id = id;
		this.age = age;
		this.name = name;
		this.characterObject = characterObject;
	}
}

