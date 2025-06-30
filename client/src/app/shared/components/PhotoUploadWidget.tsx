import { CloudUpload } from "@mui/icons-material";
import { Box, Button, Grid2, Typography } from "@mui/material";
import { useCallback, useEffect, useRef, useState } from "react";
import { useDropzone } from 'react-dropzone';
import Cropper, { ReactCropperElement } from "react-cropper";
import "cropperjs/dist/cropper.css";


type Props = {
  uploadPhoto: (file: Blob) => void;
  loading: boolean;
}


export default function PhotoUploadWidget({ uploadPhoto, loading }: Props) {

  const [files, setFiles] = useState<object & { preview: string; }[]>([]);
  const cropperRef = useRef<ReactCropperElement>(null);

  useEffect(() => {
    return () => {
      files.forEach(file => URL.revokeObjectURL(file.preview))
    }
  }, [files]);


  // We did this in [order] to [Display] the [Image]
  const onDrop = useCallback((acceptedFiles: File[]) => {
    /*
       1) I'm going threw the [Array] [acceptedFiles[]]. VVV
       2) Then from it I'm taking a [file]. And then I'm [assigning] [Object.assign()] to that [file]. VVV
       3) An [Object {}] that has a [preview] of that [URL.createObjectURL] which is baiscally a [string]. VVV
       4) And in that [string] I'm [Changing] the [file] to be a [Blob]. Because the [URL.createObjectURL] [Requires] a [Blob] [Object].
    */
    setFiles(acceptedFiles.map(file => Object.assign(file, {
      preview: URL.createObjectURL(file as Blob)
    })))
  }, [])


  const onCrop = useCallback(() => {
    const cropper = cropperRef.current?.cropper;
    cropper?.getCroppedCanvas().toBlob(blob => {
      uploadPhoto(blob as Blob)
    })
  }, [uploadPhoto])


  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop })


  return (
    <Grid2 container spacing={3}>
      <Grid2 size={4}>
        <Typography variant="overline" color="secondary">Step 1 - Add Photo</Typography>

        <Box {...getRootProps()}
          sx={{
            border: 'dashed 3px #eee',
            borderColor: isDragActive ? 'green' : '#eee',
            borderRadius: '5px',
            paddingTop: '30px',
            textAlign: 'center',
            height: '280px'
          }}

        >
          <input {...getInputProps()} />
          <CloudUpload sx={{ fontSize: 80 }} />
          <Typography variant="h5">Drop Image Here</Typography>
        </Box>
      </Grid2>

      <Grid2 size={4}>
        <Typography variant="overline" color="secondary">Step 2 - Resize Image</Typography>

        {files[0]?.preview &&
          <Cropper
            src={files[0]?.preview}
            style={{ height: 300, width: '90%' }}
            initialAspectRatio={1}
            aspectRatio={1}
            preview='.img-preview'
            guides={false}
            viewMode={1}
            background={false}
            ref={cropperRef}
          />
        }
      </Grid2>

      <Grid2 size={4}>
        {files[0]?.preview && (
          <>
            <Typography variant="overline" color="secondary">Step 3 - Preview & Upload</Typography>
            {  /* The [image] will display here because the [<div>] is [using] the 
                  [className="img-preview"] that are [<Cropper>] [Above] [using] that in it's [preview] */ }
            <div
              className="img-preview"
              style={{ width: 300, height: 300, overflow: 'hidden' }}
            />

            <Button
              sx={{ my: 1, width: 300 }}
              onClick={onCrop}
              variant="contained"
              color="secondary"
              disabled={loading}
            >
              Upload
            </Button>
          </>
        )}
      </Grid2>
    </Grid2>
  )
}