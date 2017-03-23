#ifndef _HR_HELLOWORLDMEDIATOR_H_
#define _HR_HELLOWORLDMEDIATOR_H_

#include "HrFramework/HrFrame.h"

namespace Hr
{
	class HrHelloWorldView;
	class HrHelloWorldMediator : public HrMediator
	{
	public:
		HrHelloWorldMediator();
		virtual ~HrHelloWorldMediator();


		void OnClickBtn(const HrParamPtr& param);
	protected:
		virtual void OnRegister() override;
	private:
		HrHelloWorldView* m_pHelloWorldView;
	};
}

#endif



