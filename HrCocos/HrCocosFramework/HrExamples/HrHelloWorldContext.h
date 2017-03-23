#ifndef _HR_HELLOWORLDCONTEXT_H_
#define _HR_HELLOWORLDCONTEXT_H_

#include "HrFramework/HrFrame.h"

namespace Hr
{
	class HrHelloWorldContext : public HrContext
	{
	public:
		HrHelloWorldContext();
		~HrHelloWorldContext();

		//protected:
		virtual void MapBindings() override;
	};
}

#endif



