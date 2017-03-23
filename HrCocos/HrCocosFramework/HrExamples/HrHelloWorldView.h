#ifndef _HR_HELLOWORLDVIEW_H_
#define _HR_HELLOWORLDVIEW_H_

#include "cocos2d.h"
#include "HrFramework/HrFrame.h"
#include "HrExamples/HrHelloWorldSignal.h"

namespace Hr
{
	class HrHelloWorldView : public cocos2d::Layer, public HrView
	{
	public:
		HrHelloWorldView();
		virtual ~HrHelloWorldView();

		void HrInit();

		const HrSignalPtr& GetClickBtnSignal();
	
	protected:
		HrSignalPtr m_pClickBtnSignal;
	};
}

#endif


