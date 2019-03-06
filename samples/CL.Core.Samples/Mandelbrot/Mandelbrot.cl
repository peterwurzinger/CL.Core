// voronoi kernels

//
// local memory version
//
kernel void render(__global __write_only char *outputImage)
{
    // get id of element in array
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = get_global_size(0);
    int h = get_global_size(1);

    char result = 0;
    float MinRe = -2.0f;
    float MaxRe = 1.0f;
    float MinIm = -1;
    float MaxIm = 1;
    float Re_factor = (MaxRe-MinRe)/(w-1);
    float Im_factor = (MaxIm-MinIm)/(h-1);
    float MaxIterations = 256;


    //C imaginary
    float c_im = MaxIm - y*Im_factor;

    //C real
    float c_re = MinRe + x*Re_factor;

    //Z real
    float Z_re = c_re, Z_im = c_im;

    bool isInside = true;
    bool col2 = false;
    bool col3 = false;
    int iteration =0;

    for(int n=0; n<MaxIterations; n++)
    {
        // Z - real and imaginary
        float Z_re2 = Z_re*Z_re, Z_im2 = Z_im*Z_im;

        //if Z real squared plus Z imaginary squared is greater than c squared
        if(Z_re2 + Z_im2 > 4)
        {
            if(n >= 0 && n <= (MaxIterations/2-1))
            {
                col2 = true;
                isInside = false;
                break;
            }
            else if(n >= MaxIterations/2 && n <= MaxIterations-1)
            {
                col3 = true;
                isInside = false;
                break;
            }
        }
        Z_im = 2*Z_re*Z_im + c_im;
        Z_re = Z_re2 - Z_im2 + c_re;
        iteration++;
    }
    if(col2) 
    { 
        result = iteration;
    }
    else if(col3)
    {
        result = 255;
    }
    else if(isInside)
    {
        result = 0;
    }

	int index = y * w + x;
	outputImage[index] = result;
}