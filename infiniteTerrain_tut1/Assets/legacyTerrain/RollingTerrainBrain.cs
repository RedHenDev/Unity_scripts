using UnityEngine;
using System.Collections;

class PerlinTile
{
	public GameObject pTile;
	public float genTime;


	public PerlinTile(GameObject pt, float gt)
	{
		pTile = pt;
		genTime = gt;
	}
}



public class RollingTerrainBrain : MonoBehaviour {


	public GameObject plane;
	//public GameObject plane2;
	//public GameObject plane3;
	public GameObject subject;

	public bool rollingTerrainON = true;
	private int rollCount = 0;	// Used to count if rolling terrain has been generated after Subject relocation.

	public bool makeCeiling = false;	// To turn on parenting, and to flip whole glut of tiles over head (i.e. make a ceiling).

	[Tooltip ("To correct for tiles plunging subject below.")]
	public float yOffset = 22f;

	[Tooltip ("Should match prefab; scale 1 = planeSize of 10")]
	private int planeSize;			// There are 10 subtiles per perlin tile.
	[Tooltip ("After how much movement should new Perlin Tiles be generated?")]
	public int NewTileRate = 1;	// How far does player have to move before new tiles generated?

	public int halfPtilesX = 10;	// Radius around subject. So, 20x20 here.
	public int halfPtilesZ = 10;

	Vector3 genPos;

	Hashtable ptiles = new Hashtable();

	// For initialization.
	void Start () {

		// Calculates planeSize according to scale value of prefab tile, so that tiles join correctly.
		// Each tile will have ten 'sub-tiles' of vertices.
		planeSize = (int) (plane.GetComponent<Transform>().localScale.x * 10);

		this.gameObject.transform.position = Vector3.zero;
		genPos = Vector3.zero;

		float updateTime = Time.realtimeSinceStartup;

		for(int x = -halfPtilesX; x < halfPtilesX; x++)
		{
			for(int z = -halfPtilesZ; z < halfPtilesZ; z++)
			{
		
				GameObject pt;

				Vector3 pos;
				if (makeCeiling) pos = new Vector3((x * planeSize + genPos.x), 0- yOffset, (z * planeSize + genPos.z));
				else pos = new Vector3((x * planeSize + genPos.x), 0- yOffset, (z * planeSize + genPos.z));


				if (makeCeiling) { pt = (GameObject) Instantiate(plane,pos, Quaternion.identity); 
					 }
				else
					pt = (GameObject) Instantiate(plane,pos, Quaternion.identity);

				string ptilename = "PerlinTile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
				pt.name = ptilename;
				PerlinTile ptile= new PerlinTile(pt, updateTime);
				ptiles.Add(ptilename, ptile);
			}
		}



		subject = GameObject.FindGameObjectWithTag("Subject");
		// Call adjustment of subject's starting position, so that they cannot see 'symmetry seam' of terrain.
		//subject.GetComponent<Locomotion>().StartReposition();

	}



	void Update()
	{
		if (rollingTerrainON == true || rollCount < 1)
		{
			rollCount++;	// Only generates terrain once through, to account for relocation at start. See Locomotion.

			subject = GameObject.FindGameObjectWithTag("Subject");


		// How far has subject moved?
		int MovedX = (int)(subject.transform.position.x - genPos.x);		// Difference between present position and start position?
		int MovedZ = (int)(subject.transform.position.z - genPos.z);
	
		
		if(Mathf.Abs(MovedX) >= planeSize * NewTileRate  || Mathf.Abs(MovedZ) >= planeSize * NewTileRate)
		{
			

			float updateTime = Time.realtimeSinceStartup;

			// Cast position to int, and round down to nearest ptile size.
			int subjectX = (int)(Mathf.Floor(subject.transform.position.x/planeSize)*planeSize);
			int subjectZ = (int)(Mathf.Floor(subject.transform.position.z/planeSize)*planeSize);

			for (int x = -halfPtilesX; x < halfPtilesX; x++)
			{
				for (int z = -halfPtilesZ; z < halfPtilesZ; z++)
			{
						Vector3 pos;
						if (makeCeiling) pos = new Vector3((x * planeSize + subjectX), 0- yOffset, (z * planeSize + subjectZ));
						else pos = new Vector3((x * planeSize + subjectX), 0- yOffset, (z * planeSize + subjectZ));

					string ptilename = "PerlinTile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();

					if(!ptiles.ContainsKey(ptilename))
					{

						GameObject Rplane;
						Rplane = plane;
						/*
						// Setting of plane type. This is a stand-in hack job, resulting in terrible seams, etc.
						GameObject Rplane;
						if (subject.transform.position.z > -3333f && subject.transform.position.z < 3333f) Rplane = plane;
						if (subject.transform.position.z < -3333f && subject.transform.position.z > -5777f) Rplane = plane2;
						if (subject.transform.position.z > 3333f && subject.transform.position.z < 5777f) Rplane = plane3;
						else Rplane = plane;
						*/

						planeSize = (int) (Rplane.GetComponent<Transform>().localScale.x * 10);
						this.gameObject.transform.position = Vector3.zero;
						genPos = Vector3.zero;

							GameObject pt;

							if (makeCeiling){ pt = (GameObject) Instantiate(Rplane,pos, Quaternion.identity); 
								}
								else
						pt = (GameObject) Instantiate(Rplane,pos, Quaternion.identity);
	
						pt.name = ptilename;
						PerlinTile ptile= new PerlinTile(pt, updateTime);
						ptiles.Add(ptilename, ptile);

						

					} else (ptiles[ptilename] as PerlinTile).genTime = updateTime;
	
				}
			}


			// Delete the old tiles, belonging to older time-stamp.
			Hashtable newTerraInfirma = new Hashtable();
			foreach(PerlinTile ptls in ptiles.Values)
			{
				if(ptls.genTime != updateTime)
				{
					// Destroy this Perlin-tile's game object.
					Destroy(ptls.pTile);
			} else newTerraInfirma.Add(ptls.pTile.name, ptls);
		}

		// Copy the new hashtable content into the working hashtable.
			ptiles = newTerraInfirma;

			genPos = subject.transform.position;

	}
		} // End of Rolling terrain in Update().


		}
	}
	

