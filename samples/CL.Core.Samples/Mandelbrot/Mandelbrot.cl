kernel void render(__global __write_only char *img)
{
    //local x-coordinate
    int x = get_global_id(0);
    //local y-coordinate
    int y = get_global_id(1);
    
    int width = get_global_size(0);
    int height = get_global_size(1);

    int index = y * width + x;
    
    const float minX = -2.0f;
    const float maxX = 1.0f;
    const float minY = -1.0f;
    const float maxY = 1.0f;
    const int maxIterations = 256;
    
    //-- Calculation --
    
    float cX = minX + ((float)x / width) * (maxX - minX);
    float cY = minY + ((float)y / height) * (maxY - minY);
        
    float zX = cX;
    float zY = cY;
    
    for (int i = 0; i < maxIterations; i++)
    {
        float localX = (zX * zX - zY * zY) + cX;
        float localY = (zY * zX + zX * zY) + cY;

        if ((localX * localX + localY * localY) > 4)
        {
            img[index] = i;
            return;
        }

        zX = localX;
        zY = localY;
    }
    
    img[index] = 0;
}