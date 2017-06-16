using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace EDiary
{
    /// <summary>
    /// Use this for drawing custom graphics and text with transparency.
    /// Inherit from DrawingArea and override the OnDraw method.
    /// </summary>
    abstract public class DrawingArea : Panel
    {
        /// <summary>
        /// Drawing surface where graphics should be drawn.
        /// Use this member in the OnDraw method.
        /// </summary>
        protected Graphics graphics;

        /// <summary>
        /// Override this method in subclasses for drawing purposes.
        /// </summary>
        abstract protected void OnDraw();

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Don't paint background
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Update the private member so we can use it in the OnDraw method
            this.graphics = e.Graphics;

            // Set the best settings possible (quality-wise)
            this.graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            this.graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            this.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            this.graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Calls the OnDraw subclass method
            OnDraw();

            // panel is transparent - bring it to front
            BringToFront();
        } 
    }

    public class DollarDrawer : DrawingArea
    {
        protected override void OnDraw()
        {
            // Gets the image from the global resources
            Image warermarkImage = global::EDiary.Properties.Resources.watermark;

            // Draws the two images
            this.graphics.DrawImage(warermarkImage, new Rectangle(0, 0, this.Width, this.Height));
        }
    }
}
