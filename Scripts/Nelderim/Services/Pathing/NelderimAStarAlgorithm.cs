using System;
using System.Collections.Generic;
using System.Linq;
using CalcMoves = Server.Movement.Movement;

namespace Server.PathAlgorithms
{
	public class NelderimAStarAlgorithm : PathAlgorithm
	{
		private class PathNode : IComparable<PathNode>
		{
			public Point3D pos;
			public PathNode parent;
			public int f, g;

			public PathNode(Point3D pos, PathNode parent) {
				this.pos = pos;
				this.parent = parent;
			}

			public int CompareTo(PathNode node) {
				return this.f.CompareTo(node.f);
			}

			public override bool Equals(object obj) {
				if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
					return false;
				} else {
					PathNode otherNode = (PathNode)obj;
					return pos.X == otherNode.pos.X && pos.Y == otherNode.pos.Y;
				}
			}

			public override int GetHashCode() {
				int hash = 23;
				hash = hash * 31 + pos.X;
				hash = hash * 31 + pos.Y;
				return hash;
			}

			public override string ToString() {
				return string.Format("{0},g={1},f={2}", pos.ToString(), g, f);
			}
		}

		public static int MaxRange = 20;

		public static PathAlgorithm Instance = new NelderimAStarAlgorithm();

		private static int Heuristic(PathNode start, PathNode end) {
			return (int)(Math.Pow(end.pos.X - start.pos.X, 2.0) + Math.Pow(end.pos.Y - start.pos.Y, 2.0));
		}

		public override bool CheckCondition(IPoint3D p, Map map, Point3D start, Point3D end) {
			return Utility.InRange(start, end, MaxRange);
		}

		public override Direction[] Find(IPoint3D p, Map map, Point3D start, Point3D end) {
			if (!CheckCondition(p, map, start, end)) return null;

			PathNode startNode = new PathNode(start, null);
			PathNode endNode = new PathNode(end, null);

			HashSet<PathNode> openList = new HashSet<PathNode>();
			HashSet<PathNode> closedList = new HashSet<PathNode>();
			openList.Add(startNode);
			while( openList.Any()) {
				PathNode curNode = openList.Min();
				openList.Remove(curNode);
				closedList.Add(curNode);

				if (endNode.Equals(curNode)) {
					List<PathNode> result = new List<PathNode>();
					PathNode cur = curNode;
					while (cur != null) {
						result.Add(cur);
						cur = cur.parent;
					}
					result.Reverse();
					PathNode prev = null;
					List<Direction> realResult = new List<Direction>(result.Count - 1);
					foreach (PathNode node in result) {
						if (prev != null) {
							realResult.Add(Utility.GetDirection(prev.pos, node.pos));
						}
						prev = node;
					}

					return realResult.ToArray();
				}

				List<PathNode> children = new List<PathNode>(8); // * direction around current point
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						if (x == 0 && y == 0) continue;

						int newX = curNode.pos.X + x, newY = curNode.pos.Y + y, newZ;
						Point3D newPosition = new Point3D(newX, newY, 0);
						PathNode newNode = new PathNode(newPosition, curNode);

						if (closedList.Contains(newNode) || openList.Contains(newNode)) continue;

						Direction direction = Utility.GetDirection(curNode.pos.X, curNode.pos.Y, newX, newY);

						if (!CalcMoves.CheckMovement(p, map, curNode.pos, direction, out newZ)) continue;

						newNode.pos.Z = newZ;

						if (!Utility.InRange(start, newPosition, MaxRange)) continue;

						children.Add(newNode);
					}
				}

				foreach (PathNode child in children) {
					child.g = curNode.g + 1;
					child.f = child.g + Heuristic(child, endNode);
					openList.Add(child);
				}
			}
			return null;
		}
	}
}
