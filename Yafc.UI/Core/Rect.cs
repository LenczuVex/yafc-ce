﻿using System;
using System.Numerics;

namespace Yafc.UI;

public struct Rect(float x, float y, float width, float height) {
    public float X = x, Y = y;
    public float Width = width, Height = height;

    public float Right {
        readonly get => X + Width;
        set => Width = value - X;
    }

    public float Bottom {
        readonly get => Y + Height;
        set => Height = value - Y;
    }

    public float Left {
        readonly get => X;
        set {
            Width += (X - value);
            X = value;
        }
    }

    public float Top {
        readonly get => Y;
        set {
            Height += (Y - value);
            Y = value;
        }
    }

    public static readonly Rect VeryBig = new Rect(-float.MaxValue / 2, -float.MaxValue / 2, float.MaxValue, float.MaxValue);

    public Rect(Vector2 position, Vector2 size) : this(position.X, position.Y, size.X, size.Y) { }

    public static Rect SideRect(float left, float right, float top, float bottom) => new Rect(left, top, right - left, bottom - top);

    public static Rect SideRect(Vector2 topLeft, Vector2 bottomRight) => SideRect(topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y);

    public static Rect Union(Rect a, Rect b) => SideRect(MathF.Min(a.X, a.X), MathF.Max(a.Right, b.Right), MathF.Min(a.Y, b.Y), MathF.Max(a.Bottom, b.Bottom));

    public Vector2 Size {
        readonly get => new Vector2(Width, Height);
        set {
            Width = value.X;
            Height = value.Y;
        }
    }

    public Vector2 Position {
        readonly get => new Vector2(X, Y);
        set {
            X = value.X;
            Y = value.Y;
        }
    }

    public readonly Rect RightPart(float width) => new Rect(Right - width, Y, width, Height);

    public readonly Rect LeftPart(float width) => new Rect(X, Y, width, Height);

    public readonly Vector2 TopLeft => new Vector2(X, Y);
    public readonly Vector2 TopRight => new Vector2(Right, Y);
    public readonly Vector2 BottomRight => new Vector2(Right, Bottom);
    public readonly Vector2 BottomLeft => new Vector2(X, Bottom);
    public readonly Vector2 Center => new Vector2(X + (Width * 0.5f), Y + (Height * 0.5f));

    public readonly bool Contains(Vector2 position) => position.X >= X && position.Y >= Y && position.X <= Right && position.Y <= Bottom;

    public readonly bool IntersectsWith(Rect other) => X < other.Right && Right > other.X && Y < other.Bottom && Bottom > other.Y;

    public readonly bool Contains(Rect rect) => X <= rect.X && Y <= rect.Y && Right >= rect.Right && Bottom >= rect.Bottom;

    public static Rect Intersect(Rect a, Rect b) {
        float left = MathF.Max(a.X, b.X);
        float right = MathF.Min(a.Right, b.Right);

        if (right <= left) {
            return default;
        }

        float top = MathF.Max(a.Y, b.Y);
        float bottom = MathF.Min(a.Bottom, b.Bottom);

        if (bottom <= top) {
            return default;
        }

        return SideRect(left, right, top, bottom);
    }

    public readonly bool Equals(Rect other) => this == other;

    public override readonly bool Equals(object? obj) => obj is Rect other && Equals(other);

    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    public static Rect operator +(in Rect source, Vector2 offset) => new Rect(source.Position + offset, source.Size);

    public static Rect operator -(in Rect source, Vector2 offset) => new Rect(source.Position - offset, source.Size);

    public static Rect operator *(in Rect source, float multiplier) => new Rect(source.Position * multiplier, source.Size * multiplier);

    public static bool operator ==(in Rect a, in Rect b) => a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;

    public static bool operator !=(in Rect a, in Rect b) => !(a == b);

    public override readonly string ToString() => "(" + X + "-" + Right + ")-(" + Y + "-" + Bottom + ")";

    public readonly Rect Expand(float amount) => new Rect(X - amount, Y - amount, Width + (2 * amount), Height + (2 * amount));

    public static Rect Square(Vector2 center, float side) => new Rect(center.X - (side * 0.5f), center.Y - (side * 0.5f), side, side);
    public static Rect Square(float centerX, float centerY, float side) => new Rect(centerX - (side * 0.5f), centerY - (side * 0.5f), side, side);
}
