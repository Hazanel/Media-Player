using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DSPMediaPlayer
{
    #region emum

    //public  enum MAX_NUM_OF_PLANES{Min =  3, Max = Min};
    public enum enProcessNodeRetValType_t
    {

        enProcessNodeRetCodeSuccess,
        enRetCodeError,
        enRetCodeGeneralErr,
        enProcessNodeRetRTPSolutionErr,
        enProcessNodeRetAudioFinishPlaying
    };


    public enum enDspCodec
    {
	enDspCodecNull          = 0,
	enDspCodecVP8           = 1,
	enDspCodecH263          = 2,        // rfc2190
	enDspCodecH264          = 4,
    enDspCodecJpeg          = 8,
	enDspCodecH263P         = 0x10, // rfc2429
	enDspCodecH264TSVC      = 0x20,  // H264 w/TSVC support
	enDspCodecRTVideo       = 0x40,  // RTVideo
	enDspCodecWebCYUV       = 0x80,
    enDspCodecYUV           = 0x100,
    enDspCodecG711A          = 0x200,
    enDspCodecG722          = 0x400,
    enDspCodecAACLC         = 0x800,
    enDspCodecAACLD         = 0x1000,
    enDspCodecG7221         = 0x2000,
    enDspCodecG7221C        = 0x4000,
    enDspCodecG711U         = 0x8000,
    enDspCodecG729          = 0x10000,
    enDspCodecOpus          = 0x20000,
    enDspCodecRaw           = 0x40000,
    enDspCodecWav           = 0x80000,
    enDspCodecSiren14       = 0x100000,
	enDspCodecInvalid   = 0X7FFFFFFF
    };

    public enum enPlaneNum_t
    {
	eY_Plane	= 0,
	eCb_Plane	= 1,
	eCr_Plane	= 2
    } ;

    public enum enThreadsState_t
    {
        enThreadstate_Not_Initialized,
        enThreadstate_In_Progress,
        enThreadstate_Pending_For_Destroy
    };
    #endregion enum

    public unsafe struct Frame
    {
        public byte*[] YCbCrBuff;
        public int  FrameWidth;
        public int  FrameHeight;
        public int  FrameFPS;
        public UInt32 FrameTimeStamp;
        public UInt32 m_Profile_Idc;
        public bool mb_IsFrameIntra;
        public UInt32 m_NumFramesUntilIntra;
        public bool mb_Avc_Tsvc_GopStruct;
        public string m_SharpIndication;

    };
    public  struct CommonInArgs
    {
        public string     mFileName;
        
        public enDspCodec mCodectype;
        public int        mCookie;
    };

    public delegate void RTPReleaseSolutionCB(ref NodeInArgs arNodeInArgs,
                                              ref NodeOutArgs arNodeOutArgs);

    public unsafe struct RTPInArgs
    {
        public void*    mRTPSolution;
        public int      mSaveOutFile;
        //CB rtp release function
        public RTPReleaseSolutionCB mRTPReleaseSolutionCB; 
    };
    public unsafe struct RTPOutArgs
    {
        public void* mRTPSolution;
    };

    public struct Analyzer_IN_Params
    {
        public int mPSNR_Threshold;
        public int mPitchSkipThreshold;
        public int m_CB_BackgroundColor_Value;
        public int m_CR_BackgroundColor_Value;
    };

    public unsafe struct CALInArgs
    {
        public int mbSaveRawOutFile;
        public int mbAnalyzerActive;
        public enDspCodec mCodecType;
        public Analyzer_IN_Params m_Analyzer_IN_Params;
    };


    #region ExtraInfo

    public struct t_H264_SPS_OBJ
    {
        public bool mbValid;

        public UInt32 m_profile_idc;
        public UInt32 m_constraint_set0_flag;
        public UInt32 m_constraint_set1_flag;
        public UInt32 m_constraint_set2_flag;
        public UInt32 m_constraint_set3_flag;
        public UInt32 m_reserved_zero_4bits;
        public UInt32 m_level_idc;
        public UInt32 m_seq_parameter_set_id;
        public UInt32 m_log2_max_frame_num_minus4;
        public UInt32 m_pic_order_cnt_type;
        public UInt32 log2_max_pic_order_cnt_lsb_minus4;
        public UInt32 m_num_ref_frames;
        public UInt32 m_gaps_in_frame_num_value_allowed;
        public UInt32 m_pic_width_in_mbs_minus_1;
        public UInt32 m_pic_height_in_map_units_minus_1;
        public UInt32 m_frame_mbs_only_flag;
        public UInt32 m_direct_8x8_inference_flag;
        public UInt32 m_frame_cropping_flag;
        public UInt32 m_vui_prameters_present_flag;
        public UInt32 m_rbsp_stop_one_bit;
        public UInt32 m_delta_pic_order_always_zero_flag;
        public UInt32 m_offset_for_non_ref_pic;
        public UInt32 m_log2_max_pic_order_cnt_lsb_minus4;
        public UInt32 m_offset_for_top_to_bottom_field;
        public UInt32 m_gaps_in_frame_num_value_allowed_flag;
        public UInt32 m_mb_adaptive_frame_field_flag;
        public UInt32 m_frame_crop_left_offset;
        public UInt32 m_frame_crop_right_offset;
        public UInt32 m_frame_crop_top_offset;
        public UInt32 m_frame_crop_bottom_offset;


        //VUI
        public UInt32 m_aspect_ratio_info_present_flag;
        public UInt32 m_aspect_ratio_idc;
        public UInt32 m_sar_width;
        public UInt32 m_sar_height;


    };


    public struct t_H264_PPS_OBJ
    {
        public bool mbValid;

        public UInt32 m_pic_parameter_set_id;
        public UInt32 m_seq_parameter_set_id;
        public UInt32 m_entropy_coding_mode_flag;
        public UInt32 m_bottom_field_pic_order_in_frame_present_flag;
        public UInt32 m_num_slice_groups_minus1;

    };


    public struct t_H264_SPS_PPS_InfoObj
    {
        public t_H264_SPS_OBJ mSpsObj;

        public t_H264_PPS_OBJ mPpsObj;

    } ;

    public struct t_H263_InfoObj
    {
        public bool mbValid;
        public UInt32 m_TR;
        public UInt32 m_PTYPE;
        public UInt32 m_SrcFormat;
        public UInt32 m_INTRA;
        public UInt32 m_UFEP;
        public UInt32 m_ExtendedSrcFormat;
        public UInt32 m_UFEP_extra15bits;
        public UInt32 m_PictureTypeCode;
        public UInt32 m_PlusTypeExtra6Bits;
        public UInt32 m_CPM_PSBI;
        public UInt32 m_CPFMT_PAR;
        public UInt32 m_CPFMT_PictureWidth;
        public UInt32 m_CPFMT_PictureHeigth;
        public UInt32 m_EPAR;

    } ;


    public struct t_VPX_InfoObj
    {
        public bool mbValid;
        public UInt32 m_INTRA;
        public UInt32 m_VER;
        public UInt32 m_H;
        public UInt32 m_PacketSize;
        public UInt32 m_SPC;
        public UInt32 m_ResW_scaleFactor;
        public UInt32 m_ResW;
        public UInt32 m_ResH_scaleFactor;
        public UInt32 m_ResH;

    };

    [System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
    public struct t_ExtraSolutionInfo
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        //H264 extra Info object
        public t_H264_SPS_PPS_InfoObj mH264ExtraInfo;

        [System.Runtime.InteropServices.FieldOffset(0)]
        //H263 extra Info object
        public t_H263_InfoObj mH263ExtraInfo;

        [System.Runtime.InteropServices.FieldOffset(0)]
        //VPX extra Info object
        public t_VPX_InfoObj mVPXExtraInfo;

    } ;

    #endregion ExtraInfo


    public  struct CALOutArgs
    {
        public Frame mFrame;
    };
    public unsafe struct NodeInArgs
    {
        public CommonInArgs mCommonArgs;
        public RTPInArgs mRTPInArgs;
        public CALInArgs mCALInArgs;
    };
    
    public  struct NodeOutArgs
    {       
        public CALOutArgs mCALOutFrame;

        public t_ExtraSolutionInfo mExtraInfoObj;
        
        public Process_To_Display_Queue m_TaskInQueue;
    };

    public abstract class Node_Interface
    {
        public abstract void Node_Create();
        public abstract void Node_Destroy();

        public abstract enProcessNodeRetValType_t Node_Process(ref NodeInArgs arNodeInArgs, 
                                                               ref NodeOutArgs arNodeOutArgs);

    }
}
