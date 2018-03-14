namespace GDManager.Core
{
    /// <summary>
    /// Helper functions for <see cref="IconType"/>
    /// </summary>
    public static class IconTypeExtensions
    {
        /// <summary>
        /// Converts <see cref="IconType"/> to a FontAwesome string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToFontAwesome(this IconType type)
        {
            switch (type)
            {
                case IconType.Home:
                    return "\uf015";

                case IconType.Settings:
                    return "\uf013";

                case IconType.InfoCircle:
                    return "\uf05a";

                // If none found, return null
                default:
                    return null;
            }
        }
    }
}
