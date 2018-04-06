using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PaintTable {

	private GameObject m_drawerItem;
	private PoolGameObject m_drawerPool;
	private List<GameObject> m_lDrawerInScene = new List<GameObject>();

	private Camera m_DrawCamera;

	public PaintTable(Camera camera)
	{
		m_DrawCamera = camera;
		m_drawerItem = (GameObject)Resources.Load("Prefabs/DrawerPref");
		m_drawerPool = new PoolGameObject(m_drawerItem, 5);
	}

	public DrawerTool GetDrawerTool()
	{
		GameObject tempElemfromPool = m_drawerPool.getElement ();
		m_lDrawerInScene.Add (tempElemfromPool);
		DrawerTool tempDrawerTool = new DrawerTool (tempElemfromPool, this);
		return tempDrawerTool;
	}
	
	public void ClearDrawLine()
	{	
		for (int i=0; i<m_lDrawerInScene.Count; ++i) {
			if (m_lDrawerInScene[i].tag == "Drawer")
				m_lDrawerInScene[i].GetComponent<LineRenderer>().SetVertexCount(0);
		}
	}

	public void ClearTable() 
	{
		ClearDrawLine ();
		m_drawerPool.DisableAll ();
		m_lDrawerInScene.Clear ();
	}

	public class DrawerTool 
	{
		GameObject m_drawerTool;
		PaintTable m_painTable;

		private Color m_DrawColorStart = Color.black;
		private Color m_DrawColorEnd = Color.blue;
		private float m_DrawWidthStart = 0.5f;
		private float m_DrawWidthEnd = 0.5f;

		private List<Vector3> drawPoints = new List<Vector3>();

		public DrawerTool(GameObject tool, PaintTable paintTable)
		{
			m_drawerTool = tool;
			m_painTable = paintTable;
		}

		public void Draw (Vector3 startPosition, Vector3 endPosition)
		{
			LineRenderer lineRenderer = m_drawerTool.GetComponent<LineRenderer> ();
			Vector3 mouseWorldStart = m_painTable.m_DrawCamera.ScreenToWorldPoint(startPosition);
			Vector3 mouseWorldCurrent = m_painTable.m_DrawCamera.ScreenToWorldPoint(endPosition);
			mouseWorldStart.z = m_painTable.m_DrawCamera.nearClipPlane;
			mouseWorldCurrent.z = m_painTable.m_DrawCamera.nearClipPlane;
			lineRenderer.SetColors (m_DrawColorStart, m_DrawColorEnd);
			lineRenderer.SetWidth (m_DrawWidthStart,m_DrawWidthEnd);
			if (!drawPoints.Contains (mouseWorldCurrent)) {
				drawPoints.Add (mouseWorldCurrent);
				lineRenderer.SetVertexCount (drawPoints.Count);
				lineRenderer.SetPosition (drawPoints.Count - 1, mouseWorldCurrent);	
			}
		}

		public void EndDraw()
		{
			m_drawerTool.SetActive (false);
			m_drawerTool.GetComponent<LineRenderer> ().SetVertexCount (0);
		}

		public void setColor (Color startColor, Color endColor) {
			m_DrawColorStart = startColor;
			m_DrawColorEnd = endColor;
		}
		
		public void setWidth (float startingWidth, float endWidth) {
			m_DrawWidthStart = startingWidth;
			m_DrawWidthEnd = endWidth;
		}
	}
}

